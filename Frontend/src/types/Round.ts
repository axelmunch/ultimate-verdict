import { type Option } from './Option';
import { type Result } from './Result';

export interface Round {
  id: number;
  voteId: number;
  name: string;
  startTime: number;
  endTime: number;

  options: Option[];

  result: Result | null;
}
