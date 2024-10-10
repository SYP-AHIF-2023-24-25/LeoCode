import { CodeSection} from './code-sections';
export interface ExerciseDto {
    name:string,
    creator:string,
    description:string,
    language:string,
    tags:string[],
    arrayOfSnippets: CodeSection[]
    dateCreated: Date,
    dateUpdated: Date
}