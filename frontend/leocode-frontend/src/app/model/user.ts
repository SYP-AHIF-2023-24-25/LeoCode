import { Exercise } from './exercise';

export interface User {
    username: string;
    password: string;
    exercises: Exercise[];
}