import { CodeSection} from './code-sections';
import { User } from './user';

export interface Exercise {
    name: string,
    creator: string;
    description: string;
    language: string;
    tags: string[];
    zipFile: File | null;
    emptyZipFile: File | null;
    arrayOfSnippets: CodeSection[]
    dateCreated: Date;
    dateUpdated: Date;
    teacher: string;
}