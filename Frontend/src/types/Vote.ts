import { type Option } from "./Option";
import { type Result } from "./Result";
import { type Round } from "./Round";

export interface Vote {
  id: number;
  name: string;
  description: string;

  visibility: "public" | "private";

  type: "plural" | "ranked" | "weighted" | "elo";
  nbRounds: number;
  winnersByRound: number[]; // Same size as nbRounds

  victoryCondition:
    | "none"
    | "majority"
    | "absolute majority"
    | "2/3 majority"
    | "last man standing";
  replayOnDraw: boolean;

  rounds: Round[];
  options: Option[];

  result: Result | null;
}
