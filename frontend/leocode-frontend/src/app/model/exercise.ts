export interface Exercise {
    name: string,
    instruction: string;
    language: string;
    tags: string[];
    zipFile: File | null;
    emptyZipFile: File | null;
}