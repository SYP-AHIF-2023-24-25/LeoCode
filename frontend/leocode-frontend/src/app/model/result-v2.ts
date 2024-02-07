import { Summary } from './summary';
import { TestResults } from './test-results';

export interface ResultV2 {
    Summary: Summary;
    TestResults: TestResults[];
}