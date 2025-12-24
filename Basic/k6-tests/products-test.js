import http from "k6/http";
import { check, sleep } from "k6";
import { Trend, Counter } from "k6/metrics";

// Thresholds för både performance, cache och lokala metrics
export const options = {
  vus: 50,
  duration: "3m",
  thresholds: {
    http_req_duration: ["p(95)<500"], // 95% av requests < 500ms
    http_req_failed: ["rate<0.01"], // max 1% får faila
    cache_hit: ["count>1000"], // minst 1000 cacheträffar under testet
    cache_miss: ["rate<0.5"], // max 50% får vara missar
    rate_limited: ["count<100"], // max 100 rate-limit events (429) tillåtna
    query_time: ["p(95)<300"], // 95% av queries < 300ms
    db_roundtrips: ["count<1000"], // max 1000 DB roundtrips
  },
};

const BASE_URL = "http://localhost:5001/api";

// 📊 Custom metrics
const cacheHit = new Counter("cache_hit");
const cacheMiss = new Counter("cache_miss");
const rateLimited = new Counter("rate_limited");
const queryTime = new Trend("query_time");
const dbRoundtrips = new Counter("db_roundtrips");

export default function () {
  const page = Math.floor(Math.random() * 5) + 1;
  const pageSize = 10;

  const res = http.get(
    `${BASE_URL}/Products?page=${page}&pageSize=${pageSize}`,
    {
      headers: { Accept: "application/json" },
    }
  );

  // Cacheträffar (Age-header om du har cache på lokalt API)
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

  // Simulerade lokala metrics (ersätt med riktiga värden från API-svaret)
  queryTime.add(Math.random() * 300);
  dbRoundtrips.add(Math.floor(Math.random() * 3) + 1);

  sleep(1); // simulera användarens paus
}
