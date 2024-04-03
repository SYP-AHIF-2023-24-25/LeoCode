export interface Exercise {
    instruction: string;
    language: string;
    tags: string[];
    zipFile: File | null;
    emptyZipFile: File | null;
}