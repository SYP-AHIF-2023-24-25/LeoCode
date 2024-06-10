import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExerciseDto } from '../model/exerciseDto';
import { Params } from '@angular/router';
import { DbService } from '../service/db-service.service';

@Component({
  selector: 'app-introduction',
  templateUrl: './introduction.component.html',
  styleUrls: ['./introduction.component.css']
})
export class IntroductionComponent {

  userName: string|null = sessionStorage.getItem('userName');
  exerciseName: string | null = sessionStorage.getItem('exerciseName');

  exercise : ExerciseDto = {
    name: "",
    description: "",
    language: "",
    tags: [],
    arrayOfSnippets: []
  }
  constructor( private route: ActivatedRoute, private restDb: DbService) {
  }

  ngOnInit(): void {
    if(this.userName != null && this.exerciseName != null){
      this.restDb.getExerciseByUsername(this.userName, this.exerciseName).subscribe((data: ExerciseDto[]) => {
        this.exercise = data[0];
        console.log(this.exercise.name);
        console.log(this.exercise.arrayOfSnippets);
        console.log(this.exercise.language);
        console.log(this.exercise.description);
    });
    }
  }
}
