import { Stats } from './stats';
import { Test } from './test';
export interface OriginalResult {
    stats: Stats;
    tests: Test[];
    pending: any[];
    failures: any[];
    passes: Test[];
}
