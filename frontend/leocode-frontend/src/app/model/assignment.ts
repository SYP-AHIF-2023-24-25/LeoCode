import { Exercise } from "./exercise";
import { User } from "./user";

export interface Assignment{
    exercise: Exercise | null,
    exerciseId: number,
    dueDate: Date,
    students: User[],
    teacherId: number,
    teacher: User| null,
    name: string
}