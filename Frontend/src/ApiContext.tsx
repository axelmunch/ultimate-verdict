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

const votes: Vote[] = [
  {
    id: 1,
    name: "Test Vote",
    description: "This is a test vote",
    visibility: "public",
    type: "plural",
    nbRounds: 3,
    winnersByRound: [4, 2, 1],
    victoryCondition: "absolute majority",
    replayOnDraw: true,
    rounds: [
      {
        id: 1,
        voteId: 1,
        name: "First Round",
        startTime: Date.now(),
        endTime: Date.now() + 1000 * 60 * 60,
        options: [
          { id: 10, name: "Option A" },
          { id: 3, name: "Option B" },
          { id: 1, name: "Option C" },
          { id: 100, name: "Option D" },
          { id: 6, name: "Option E" },
        ],
        result: {
          id: 1,
          options: [
            { id: 10, name: "Option A" },
            { id: 3, name: "Option B" },
            { id: 1, name: "Option C" },
            { id: 100, name: "Option D" },
            { id: 6, name: "Option E" },
          ],
          scores: [10, 5, 3, 10, 1],
          res: "draw",
        },
      },
      {
        id: 2,
        voteId: 1,
        name: "First Round",
        startTime: Date.now(),
        endTime: Date.now() + 1000 * 60 * 60,
        options: [
          { id: 10, name: "Option A" },
          { id: 100, name: "Option D" },
        ],
        result: null,
      },
    ],
    options: [
      { id: 10, name: "Option A" },
      { id: 3, name: "Option B" },
      { id: 1, name: "Option C" },
      { id: 100, name: "Option D" },
      { id: 6, name: "Option E" },
    ],
    result: null,
  },
  {
    id: 2,
    name: "Another Test Vote",
    description: "This is another test vote",
    visibility: "public",
    type: "ranked",
    nbRounds: 2,
    winnersByRound: [2, 1],
    victoryCondition: "majority",
    replayOnDraw: false,
    rounds: [
      {
        id: 3,
        voteId: 2,
        name: "1st",
        startTime: Date.now(),
        endTime: Date.now() + 1000 * 60 * 60,
        options: [
          { id: 100, name: "Element A" },
          { id: 1000, name: "Element D" },
        ],
        result: null,
      },
    ],
    options: [
      { id: 100, name: "Element A" },
      { id: 30, name: "Element B" },
      { id: 10, name: "Element C" },
      { id: 1000, name: "Element D" },
      { id: 60, name: "Element E" },
    ],
    result: null,
  },
  {
    id: 3,
    name: "The vote is finished",
    description: "",
    visibility: "private",
    type: "plural",
    nbRounds: 1,
    winnersByRound: [1],
    victoryCondition: "majority",
    replayOnDraw: true,
    rounds: [
      {
        id: 4,
        voteId: 2,
        name: "1st",
        startTime: Date.now() - 1000 * 60 * 60 * 2,
        endTime: Date.now() - 1000 * 60 * 60,
        options: [
          { id: 128, name: "aaa" },
          { id: 64, name: "bbb" },
          { id: 32, name: "ccc" },
        ],
        result: {
          id: 2,
          options: [
            { id: 128, name: "aaa" },
            { id: 64, name: "bbb" },
            { id: 32, name: "ccc" },
          ],
          scores: [22, 40, 40],
          res: "draw",
        },
      },
      {
        id: 5,
        voteId: 2,
        name: "1st",
        startTime: Date.now() - 1000 * 60 * 60,
        endTime: Date.now() - 1,
        options: [
          { id: 64, name: "bbb" },
          { id: 32, name: "ccc" },
        ],
        result: {
          id: 3,
          options: [
            { id: 64, name: "bbb" },
            { id: 32, name: "ccc" },
          ],
          scores: [30, 42],
          res: "winner",
        },
      },
    ],
    options: [
      { id: 128, name: "aaa" },
      { id: 64, name: "bbb" },
      { id: 32, name: "ccc" },
    ],
    result: {
      id: 3,
      options: [
        { id: 64, name: "bbb" },
        { id: 32, name: "ccc" },
      ],
      scores: [30, 42],
      res: "winner",
    },
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

    return apiRequest("votes", "GET")
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

  const createVote = async (vote: VoteInput): Promise<number> => {
    votes.push({
      ...vote,
      id: votes.length + 1,
      rounds: [],
      options: vote.options.map((option, index) => ({
        name: option,
        id: index + 1 + votes.reduce((acc, v) => acc + v.options.length, 0),
      })),
      result: null,
    });
    return new Promise((resolve) => {
      resolve(votes.length);
    });

    return apiRequest(`vote`, "POST", vote)
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
