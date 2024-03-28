import { Result } from './result';
export interface Value {
    value: Result
    formatters: any[],
    contentTypes: any[],
    declaredType: any,
    statusCode: any
}