export interface Exercise {
    name: string,
    creator: string;
    instruction: string;
    language: string;
    tags: string[];
    zipFile: File | null;
    emptyZipFile: File | null;
    dateCreated: Date;
    dateUpdated: Date;
}