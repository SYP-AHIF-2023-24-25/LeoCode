import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ExerciseDto } from '../model/exerciseDto';
import { Params } from '@angular/router';
import { DbService } from '../service/db-service.service';
import { Exercise } from '../model/exercise';

@Component({
  selector: 'app-introduction',
  templateUrl: './introduction.component.html',
  styleUrls: ['./introduction.component.css']
})
export class IntroductionComponent {

  userName: string|null = sessionStorage.getItem('userName');
  exerciseName: string | null = sessionStorage.getItem('exerciseName');
  date: Date = new Date();
  creator: string | undefined = "";

    exercise : Exercise={
      name: "",
      creator: "",
      description: "",
      language: "",
      tags: [],
      zipFile: null,
      emptyZipFile: null,
      arrayOfSnippets:[],
      dateCreated: new Date(),
      dateUpdated: new Date(),
      teacher: ""
    };

  constructor( private route: ActivatedRoute, private restDb: DbService, private router: Router) {
  }

  ifUserName: string | null = '';
  async logout(): Promise<void> {
    sessionStorage.setItem('shouldLogOut', 'true');
    this.router.navigate(['/login']);
  }

  ngOnInit(): void {
    this.ifUserName = sessionStorage.getItem('ifUserName');

    this.route.queryParams.subscribe((params: Params) => {
      this.creator = params['creator'];   
    });
    
    console.log(this.creator);
    console.log(this.exerciseName);

    if(this.creator != null && this.exerciseName != null){
      this.restDb.getExerciseByUsername(this.creator, this.exerciseName).subscribe((data: Exercise[]) => {
        this.exercise = data[0];
       
        console.log(this.exercise.name);
        console.log(this.exercise.arrayOfSnippets);
        console.log(this.exercise.language);
        console.log(this.exercise.description);
    });
    }
  }
}
