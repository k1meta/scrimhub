export type HealthBody = {
  status: "ok";
  service: string;
};

export function healthBody(service: string): HealthBody {
  return { status: "ok", service };
}
