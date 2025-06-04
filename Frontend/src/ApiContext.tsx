import {
  createContext,
  useContext,
  useState,
} from "react";
import type { ReactNode } from "react";
import ErrorToast from "./ErrorToast";
import apiRequest from "./apiRequest";

type ApiContextType = {
  test: () => Promise<unknown>;
};

const ApiContext = createContext<ApiContextType | undefined>(undefined);

function ApiProvider({ children }: { children: ReactNode }) {
  const [showError, setShowError] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");

  const handlePromiseError = (error: unknown) => {
    if (error instanceof Error) {
      setErrorMessage(error.message);
    } else {
      setErrorMessage(
        typeof error === "object" && error !== null && "toString" in error
          ? error.toString()
          : String(error)
      );
    }
    console.error(error);
    setShowError(true);
  };

  const test = async (): Promise<unknown> => {
    return apiRequest("weatherforecast", undefined, {'a':[1, true, "b"], 2:3}).catch(handlePromiseError);
  };

  return (
    <ApiContext.Provider value={{ test }}>
      {children}
      <ErrorToast
        open={showError}
        message={errorMessage}
        onClose={() => {
          setShowError(false);
        }}
      />
    </ApiContext.Provider>
  );
}

export const useApi = (): ApiContextType => {
  const context = useContext(ApiContext);
  if (!context) {
    throw new Error(`useApi must be used within a ${ApiProvider.name}`);
  }
  return context;
};

export default ApiProvider;
