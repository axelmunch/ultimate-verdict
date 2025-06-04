import { type Option } from "./Option";

export interface Result {
    id: number;
    options: Option[];
    scores: number[];  // Same size as options
    res: "winner" | "draw" | "inconclusive";
}
