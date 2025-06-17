import type { Decision } from "./Decision";
import type { Option } from "./Option";

export interface VotingSystemProps {
  options: Option[];
  setCanSubmit: (canSubmit: boolean) => void;
  setDecisions: (decisions: Decision[]) => void;
}
