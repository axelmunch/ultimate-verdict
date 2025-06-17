import { createContext, useContext, useState } from "react";
import type { ReactNode } from "react";
import ErrorToast from "./ErrorToast";
import apiRequest from "./apiRequest";
import type { Decision, Vote, VoteInput } from "./types";

type ApiContextType = {
  test: () => Promise<unknown>;
  getVotes: () => Promise<Vote[]>;
  getVote: (voteId: number) => Promise<Vote>;
  createVote: (vote: VoteInput) => Promise<number>;
  submitDecision: (roundId: number, decisions: Decision[]) => Promise<unknown>;
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
          : String(error),
      );
    }
    setShowError(true);
    throw error;
  };

  const test = async (): Promise<unknown> => {
    return apiRequest("").catch(handlePromiseError);
  };

  const getVotes = async (): Promise<Vote[]> => {
    return apiRequest("Vote/GetVote", "GET")
      .then((data) => data as Vote[])
      .catch(handlePromiseError);
  };

  const getVote = async (voteId: number): Promise<Vote> => {
    return apiRequest(`Vote/${voteId}`, "GET")
      .then((data) => data as Vote)
      .catch(handlePromiseError);
  };

  const createVote = async (vote: VoteInput): Promise<number> => {
    return apiRequest(`Vote/CreateVote`, "POST", vote)
      .then((data) => data as number)
      .catch(handlePromiseError);
  };

  const submitDecision = async (
    roundId: number,
    decisions: Decision[],
  ): Promise<unknown> => {
    return apiRequest("decision", "POST", {
      roundId,
      decisions,
    }).catch(handlePromiseError);
  };

  return (
    <ApiContext.Provider
      value={{ test, getVotes, getVote, createVote, submitDecision }}
    >
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
