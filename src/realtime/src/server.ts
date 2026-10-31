import { createServer } from "node:http";
import { healthBody } from "./health.js";

const port = Number(process.env.PORT ?? 3001);

const server = createServer((request, response) => {
  if (request.method === "GET" && request.url === "/health") {
    response.writeHead(200, { "content-type": "application/json; charset=utf-8" });
    response.end(JSON.stringify(healthBody("realtime")));
    return;
  }

  response.writeHead(404, { "content-type": "application/json; charset=utf-8" });
  response.end(JSON.stringify({ status: "not_found", service: "realtime" }));
});

server.listen(port, () => {
  console.log(`realtime listening on ${port}`);
});
