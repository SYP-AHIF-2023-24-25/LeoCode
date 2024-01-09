export interface Test {
    title: string;
    fullTitle: string;
    file: string;
    duration: number;
    currentRetry: number;
    speed: string;
    err: object;
}
