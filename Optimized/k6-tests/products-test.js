import http from "k6/http";
import { check, sleep } from "k6";
import { Counter } from "k6/metrics";

// Thresholds för både performance och cache
export const options = {
  vus: 50, // antal samtidiga användare
  duration: "3m", // testets längd
  thresholds: {
    http_req_duration: ["p(95)<500"], // 95% av requests < 500ms
    http_req_failed: ["rate<0.01"], // max 1% får faila
    cache_hit: ["count>1000"], // minst 1000 cacheträffar under testet
    cache_miss: ["rate<0.5"], // max 50% får vara missar
    rate_limited: ["count<100"], // max 100 rate-limit events (429) tillåtna
  },
};

const BASE_URL = "https://ecommerce-apim-dev.azure-api.net";
const SUBSCRIPTION_KEY = "text-your-subscription-key-here";

// 📊 Custom metrics
const cacheHit = new Counter("cache_hit");
const cacheMiss = new Counter("cache_miss");
const rateLimited = new Counter("rate_limited");

export default function () {
  const page = Math.floor(Math.random() * 5) + 1;
  const pageSize = 10;

  const res = http.get(
    `${BASE_URL}/products?page=${page}&pageSize=${pageSize}`,
    {
      headers: {
        "Ocp-Apim-Subscription-Key": SUBSCRIPTION_KEY,
        Accept: "application/json",
      },
    }
  );

  // Cacheträffar
  if (res.headers["Age"]) {
    cacheHit.add(1);
  } else {
    cacheMiss.add(1);
  }

  // Rate limiting (429 Too Many Requests)
  if (res.status === 429) {
    rateLimited.add(1);
  }

  // Checks
  check(res, {
    "status is 200": (r) => r.status === 200,
    "response time < 500ms": (r) => r.timings.duration < 500,
  });

  sleep(1); // simulera användarens paus
}
