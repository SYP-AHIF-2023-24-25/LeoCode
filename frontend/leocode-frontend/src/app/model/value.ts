import { ResultV2 } from './result-v2';
export interface Value {
    value: ResultV2
    formatters: any[],
    contentTypes: any[],
    declaredType: any,
    statusCode: any
}