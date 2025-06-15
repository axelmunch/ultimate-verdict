export type HTTPMethod = "GET" | "POST" | "PUT" | "PATCH" | "DELETE";

export default function apiRequest(
  route: string,
  method: HTTPMethod = "GET",
  data?: unknown,
): Promise<unknown> {
  const BACKEND_URL = "http://localhost:5240";

  const options: RequestInit = {
    method,
    headers: {
      "Content-Type": "application/json",
    },
  };

  if (method === "GET" && data) {
    const queryParams = new URLSearchParams(data as Record<string, string>);
    route += `?${queryParams.toString()}`;
  }

  if (method !== "GET" && data) {
    options.body = JSON.stringify(data);
  }

  return fetch(`${BACKEND_URL}/${route}`, options)
    .then((response) => response.json())
    .catch((error) => {
      throw new Error(
        JSON.stringify({
          route: `${BACKEND_URL}/${route}`,
          method,
          data,
          error,
        }),
      );
    });
}
