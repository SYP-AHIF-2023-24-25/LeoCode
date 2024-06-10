import { CodeSection} from './code-sections';
export interface ExerciseDto {
    name:string,
    description:string,
    language:string,
    tags:string[],
    arrayOfSnippets: CodeSection[]
}