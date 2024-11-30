import { Component, OnInit } from '@angular/core';
import { Tags } from '../model/tags.enum';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { startWith, map } from 'rxjs/operators';
import { DbService } from '../service/db-service.service';
import { User } from '../model/user';
import { Router } from '@angular/router';
import { Exercise } from '../model/exercise';

@Component({
  selector: 'app-start-screen',
  templateUrl: './start-screen.component.html',
  styleUrls: ['./start-screen.component.css']
})

export class StartScreenComponent implements OnInit {
  ifUserName: string | null = '';
  filterMyExercises: boolean = false;  // Zustand für das Filtern nach dem Ersteller


  defaultUser: User = {
    username: 'Default User',
    password: 'password',
    exercises: [],
    isTeacher: false
  }

  constructor(private rest: DbService, private router: Router) {
    this.filteredSearchTags = this.tagCtrl.valueChanges.pipe(
      startWith(null),
      map((tag: string | null) => tag ? this._filter(tag) : this.availableTags.slice())
    );
  }

  searchQuery: string = '';

  exercises: Exercise[] = [];
  filteredSearchTags: Observable<string[]> | undefined;

  displayedColumns: string[] = ['name', 'creator', 'tags', 'dateCreated', 'dateUpdated', 'details']; 
  separatorKeysCodes: number[] = [13, 188]; // 13: Enter, 188: Komma


  selectedSearchTags: string[] = [];  
  tagCtrl = new FormControl();  

  // Verfügbare Tags
  availableTags: string[] = Object.values(Tags); 

  ngOnInit(): void {
    this.ifUserName = sessionStorage.getItem('ifUserName');
    sessionStorage.setItem("userName", this.defaultUser.username);

    this.rest.getExerciseByUsername().subscribe((data: Exercise[]) => {
      this.exercises = data;
    });
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.availableTags.filter(tag => tag.toLowerCase().includes(filterValue));
  }

  removeSearchTag(tag: string): void {
    const index = this.selectedSearchTags.indexOf(tag);
    if (index >= 0) {
      this.selectedSearchTags.splice(index, 1);
    }
  }

  selectedSearch(event: MatAutocompleteSelectedEvent): void {
    this.selectedSearchTags.push(event.option.viewValue);
    this.tagCtrl.setValue(null);
  }

  get filteredExercises() {
    return this.exercises
      .filter(exercise => {
        // Filter für den Namen der Übung
        const matchesName = this.searchQuery
          ? exercise.name.toLowerCase().includes(this.searchQuery.toLowerCase())
          : true;

        // Filter für die Tags
        const matchesTags = this.selectedSearchTags.length
          ? exercise.tags.some(tag => this.selectedSearchTags.includes(tag))
          : true;

        // Filter für 'nur meine Übungen'
        const matchesCreator = this.filterMyExercises
          ? exercise.creator === this.ifUserName  // Filtert nur Übungen, die der angemeldete Benutzer erstellt hat
          : true;

        return matchesName && matchesTags && matchesCreator;  // Alle Filter müssen zutreffen
      })
      .sort((a, b) => {
        return a.name.localeCompare(b.name);  // Sortierung nach Namen
      });
  }
  

  addExerciseNameToSessionStorage(exerciseName: string) {
    sessionStorage.setItem("exerciseName", exerciseName);
  }

  async logout(): Promise<void> {
    sessionStorage.setItem('shouldLogOut', 'true');
    this.router.navigate(['/login']);
  }
}
