export type HTTPMethod = "GET" | "POST" | "PUT" | "PATCH" | "DELETE";

const BACKEND_URL = import.meta.env.VITE_API_URL;

export default function apiRequest(
  route: string,
  method: HTTPMethod = "GET",
  data?: unknown,
): Promise<unknown> {
  let requestRoute = route.slice();

  const options: RequestInit = {
    method,
    headers: {
      "Content-Type": "application/json",
    },
  };

  if (method === "GET" && data) {
    const queryParams = new URLSearchParams(data as Record<string, string>);
    requestRoute += `?${queryParams.toString()}`;
  }

  if (method !== "GET" && data) {
    options.body = JSON.stringify(data);
  }

  return fetch(`${BACKEND_URL}/${requestRoute}`, options)
    .then((response) => response.json())
    .catch((error) => {
      throw new Error(
        JSON.stringify({
          route: `${BACKEND_URL}/${requestRoute}`,
          method,
          data,
          error,
        }),
      );
    });
}
