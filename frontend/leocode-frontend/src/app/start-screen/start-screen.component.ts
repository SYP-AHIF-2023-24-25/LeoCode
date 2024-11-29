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

  defaultUser: User = {
    username: 'Default User',
    password: 'password',
    exercises: [],
    isTeacher: false
  }

  constructor(private rest: DbService, private router: Router) {
    // Initialisiere die gefilterten Tags und die Autocomplete-Suche
    this.filteredSearchTags = this.tagCtrl.valueChanges.pipe(
      startWith(null),
      map((tag: string | null) => tag ? this._filter(tag) : this.availableTags.slice())
    );
  }

  // Property für den Suchtext der Übungsnamen
  searchQuery: string = '';

  // Property für die gefilterten Übungen
  exercises: Exercise[] = [];
  filteredSearchTags: Observable<string[]> | undefined;

  // Hier definieren wir displayedColumns für die Tabelle
  displayedColumns: string[] = ['name', 'creator', 'tags', 'dateCreated', 'dateUpdated', 'details']; 
  separatorKeysCodes: number[] = [13, 188]; // 13: Enter, 188: Komma


  selectedSearchTags: string[] = [];  // Tags, die ausgewählt wurden
  tagCtrl = new FormControl();  // FormControl für die Tags

  // Verfügbare Tags
  availableTags: string[] = Object.values(Tags);  // Ersetzen durch tatsächliche Tags

  ngOnInit(): void {
    this.ifUserName = sessionStorage.getItem('ifUserName');
    sessionStorage.setItem("userName", this.defaultUser.username);

    this.rest.getExerciseByUsername().subscribe((data: Exercise[]) => {
      this.exercises = data;
    });
  }

  // Filtert die Tags basierend auf dem Eingabewert
  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.availableTags.filter(tag => tag.toLowerCase().includes(filterValue));
  }

  // Entfernen eines Tags aus der ausgewählten Liste
  removeSearchTag(tag: string): void {
    const index = this.selectedSearchTags.indexOf(tag);
    if (index >= 0) {
      this.selectedSearchTags.splice(index, 1);
    }
  }

  // Hinzufügen eines Tags zur Liste der ausgewählten Tags
  selectedSearch(event: MatAutocompleteSelectedEvent): void {
    this.selectedSearchTags.push(event.option.viewValue);
    this.tagCtrl.setValue(null);
  }

  // Getter für die gefilterten Übungen
  get filteredExercises() {
    return this.exercises
      .filter(exercise => {
        // Übungsname filtern
        const matchesName = this.searchQuery 
          ? exercise.name.toLowerCase().includes(this.searchQuery.toLowerCase()) 
          : true;

        // Tags filtern
        const matchesTags = this.selectedSearchTags.length
          ? exercise.tags.some(tag => this.selectedSearchTags.includes(tag))
          : true;

        return matchesName && matchesTags;
      })
      .sort((a, b) => {
        // Sortiere nach Übungsnamen
        return a.name.localeCompare(b.name);
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
