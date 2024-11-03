import { Component, OnInit } from '@angular/core';
import { ExerciseDto } from '../model/exerciseDto';
import { RestService } from '../service/rest.service';
import { DbService } from '../service/db-service.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { Tags } from '../model/tags.enum';
import { map, Observable, startWith } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Exercise } from '../model/exercise';

@Component({
  selector: 'app-exercise-details',
  templateUrl: './exercise-details.component.html',
  styleUrls: ['./exercise-details.component.css']
})
export class ExerciseDetailsComponent implements OnInit {
  ifUserName: string | null = '';
  creator: string | undefined = '';
  exerciseName: string | null = "";
  currentName: string = "";
  isEditingName = false; // Bearbeitungsmodus für den Übungsnamen
  isEditingDescription = false; // Bearbeitungsmodus für die Einführung
  isEditingTag=false;
  originalDescription: string = ""; // Originalbeschreibung für Abbrechen

  zipFile: File | null = null;
  emptyZipFile: File | null = null;
  emptyZipFileUploaded: boolean = false;
  ZipFileUploaded: boolean = false;

  tagCtrl = new FormControl();
  availableTags: string[] = Object.values(Tags);
  filteredTags: Observable<string[]> | undefined;
  separatorKeysCodes: number[] = [13, 188]; // Enter und Komma

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
      teacher: undefined
    };

  constructor(
    private rest: RestService,
    private route: ActivatedRoute,
    private restDb: DbService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.filteredTags = this.tagCtrl.valueChanges.pipe(
      startWith(null),
      map((tag: string | null) => tag ? this._filter(tag) : this.availableTags.slice())
    );
  }

  async logout(): Promise<void> {
    sessionStorage.setItem('shouldLogOut', 'true');
    this.router.navigate(['/login']);
  }

  ngOnInit(): void {
    this.ifUserName = sessionStorage.getItem('ifUserName');
    console.log(this.ifUserName)
    this.route.queryParams.subscribe((params: Params) => {
      this.exerciseName = params['exerciseName'];
      this.currentName = params['exerciseName'];
      this.creator = params['creator'];

      if (this.ifUserName != null && this.exerciseName != null) {
        sessionStorage.setItem("exerciseName", this.exerciseName);
      } else {
        this.exerciseName = sessionStorage.getItem('exerciseName');
      }
    });

    if (this.ifUserName != null && this.exerciseName != null) {
      this.restDb.getExerciseByUsername(this.creator, this.exerciseName).subscribe((data: Exercise[]) => {
        this.exercise = data[0];
        console.log(this.exercise);
        console.log(this.exercise.zipFile);
        this.originalDescription = this.exercise.description; // Speichert die Originalbeschreibung für Abbrechen
      });
    }
  }

  // Bearbeiten des Übungsnamens
  editExerciseName() {
    this.isEditingName = true;
  }

  // Bearbeiten der Beschreibung
  editDescription() {
    this.isEditingDescription = true;
  }

  editTag(){
    this.isEditingTag = true;
  }

  // Tags auswählen
  toggleTagSelection(tag: string) {
    if (this.exercise.tags.includes(tag)) {
      this.exercise.tags = this.exercise.tags.filter(t => t !== tag);
    } else {
      this.exercise.tags.push(tag);
    }
  }

  addTag(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    if ((value || '').trim()) {
      this.exercise.tags.push(value.trim().toUpperCase());
      this.availableTags.push(value.trim().toUpperCase());
    }

    if (input) {
      input.value = '';
    }

    this.tagCtrl.setValue(null);
  }

  isSelectedTag(tag: string): boolean {
    return this.exercise.tags.includes(tag);
  }

  removeTag(tag: string): void {
    const index = this.exercise.tags.indexOf(tag);

    if (index >= 0) {
      this.exercise.tags.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.exercise.tags.push(event.option.viewValue);
    this.tagCtrl.setValue(null);
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.availableTags.filter(tag => tag.toLowerCase().includes(filterValue));
  }

  SaveChanges() {
    this.exercise.dateUpdated = new Date();
    this.restDb.UpdateDetails(this.exercise.creator, this.exercise.description, this.exercise.tags, this.currentName, this.exercise.name).subscribe((data: any) => {
      this.snackBar.open('Exercise successfully updated', 'Close', {
        duration: 5000,
        horizontalPosition: 'center',
        verticalPosition: 'top',
      });
    });
    this.isEditingDescription=false;
    this.isEditingName=false;
    this.isEditingTag = false;
  }

  clearEmptyZipFile() {
    this.emptyZipFile = null;
    this.emptyZipFileUploaded = false;
    console.log(this.emptyZipFile);
  }

  clearZipFile() {
    this.zipFile = null;
    this.ZipFileUploaded = false;
    console.log(this.zipFile);
  }

  uploadZipFile(event: DragEvent) {
    const file = event.dataTransfer;

    if (file && file.files && file.files.length > 0) {
      this.zipFile = file.files[0];
      this.ZipFileUploaded = true;
    } else {
      console.error('No file selected');
      this.snackBar.open('No file selected', 'Close', {
        duration: 5000,
      });
    }
  }

    uploadEmptyZipFile(event: DragEvent) {
    const file = event.dataTransfer;

    if (file && file.files && file.files.length > 0) {
      this.emptyZipFile = file.files[0];
      this.emptyZipFileUploaded = true;
    } else {

      console.error('No file selected');
      this.snackBar.open('No file selected', 'Close', {
        duration: 5000,
      });
    }
  }

  createAssignment() {
    /*this.restDb.AddAssignment(this.exerciseName!, this.ifUserName!, new Date(), 'Assignment ' + this.exerciseName).subscribe((data: any) => {
      this.snackBar.open('Assignment successfully created', 'Close', {
        duration: 5000,
        horizontalPosition: 'center',
        verticalPosition: 'top',
      });
      console.log(data);
      this.router.navigate(['/create-assignment'], { state: { assignmentData: data } });
    });*/
    let data = {
      exerciseName: this.exerciseName,
      creator: this.ifUserName,
      dateDue: new Date(),
      name: 'Assignment ' + this.exerciseName
    };
    this.router.navigate(['/create-assignment'], { state: { assignmentData: data } });
  }
}
