import { Summary } from './summary';
import { TestResults } from './test-results';

export interface Result {
    Summary: Summary;
    TestResults: TestResults[];
}