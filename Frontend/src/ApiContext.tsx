import { createContext, useContext, useState } from "react";
import type { ReactNode } from "react";
import ErrorToast from "./ErrorToast";
import apiRequest from "./apiRequest";
import type { Decision, Vote } from "./types";

type ApiContextType = {
  test: () => Promise<unknown>;
  getVotes: () => Promise<Vote[]>;
  getVote: (voteId: number) => Promise<Vote>;
  submitDecision: (roundId: number, decisions: Decision[]) => Promise<unknown>;
};

const votes: Vote[] = [
  {
    id: 1,
    name: "Test Vote",
    description: "This is a test vote",
    liveResults: false,
    visibility: "public",
    type: "plural",
    nbRounds: 3,
    winnersByRound: [4, 2, 1],
    victoryCondition: "absolute majority",
    replayOnDraw: true,
    rounds: [],
    options: [],
    result: null,
  },
  {
    id: 2,
    name: "Another Test Vote",
    description: "This is another test vote",
    liveResults: false,
    visibility: "public",
    type: "plural",
    nbRounds: 2,
    winnersByRound: [2, 1],
    victoryCondition: "majority",
    replayOnDraw: false,
    rounds: [],
    options: [],
    result: null,
  },
];

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
    return new Promise((resolve) => resolve(votes));

    return apiRequest("vote", "GET")
      .then((data) => data as Vote[])
      .catch(handlePromiseError);
  };

  const getVote = async (voteId: number): Promise<Vote> => {
    return new Promise<Vote>((resolve) => {
      if (voteId < 1 || voteId > votes.length) {
        throw new Error(`Vote with ID ${voteId} does not exist.`);
      }
      resolve(votes[voteId - 1]);
    }).catch(handlePromiseError);

    return apiRequest(`vote/${voteId}`, "GET")
      .then((data) => data as Vote)
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
    <ApiContext.Provider value={{ test, getVote, getVotes, submitDecision }}>
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
