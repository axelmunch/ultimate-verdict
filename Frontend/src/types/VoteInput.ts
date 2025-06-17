import { type Vote } from "./Vote";

export interface VoteInput
  extends Omit<Vote, "id" | "rounds" | "options" | "result"> {
  options: string[];
  startDate: number;
  roundDuration: number;
}
