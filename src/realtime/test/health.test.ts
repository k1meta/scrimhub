import assert from "node:assert/strict";
import test from "node:test";
import { healthBody } from "../src/health.ts";

test("health body names the realtime service", () => {
  assert.deepEqual(healthBody("realtime"), { status: "ok", service: "realtime" });
});
