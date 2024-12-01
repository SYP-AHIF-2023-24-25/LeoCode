import { CodeSection} from './code-sections';
import { User } from './user';
export interface ExerciseDto {
    name:string,
    creator:string,
    description:string,
    language:string,
    tags:string[],
    arrayOfSnippets: CodeSection[]
    dateCreated: Date,
    dateUpdated: Date,
    teacher: User | undefined,
}