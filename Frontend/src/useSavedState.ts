import { useEffect, useState } from "react";

export default function useSavedState<T>(
  key: string,
  initialValue: T,
  parse: (value: unknown) => T = (value) => value as T
): [T, React.Dispatch<React.SetStateAction<T>>] {
  const [value, setValue] = useState<T>(() => {
    if (typeof window !== "undefined") {
      const storedValue = localStorage.getItem(key);
      return storedValue !== null
        ? parse(JSON.parse(storedValue))
        : initialValue;
    }
    return initialValue;
  });

  useEffect(() => {
    if (typeof window !== "undefined") {
      localStorage.setItem(key, JSON.stringify(value));
    }
  }, [key, value]);

  return [value, setValue];
}
