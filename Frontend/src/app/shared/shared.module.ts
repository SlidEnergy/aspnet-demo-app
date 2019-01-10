import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import {
    MatNativeDateModule, MatDatepickerModule, MatIconModule, MatButtonModule, MatCheckboxModule, MatToolbarModule,
    MatCardModule, MatFormFieldModule, MatInputModule, MatRadioModule, MatListModule, MatSnackBarModule, MatTableModule, MatAutocompleteModule, MatProgressSpinnerModule
} from '@angular/material';

import { MatProgressButtonsModule } from 'mat-progress-buttons'

@NgModule({
    declarations: [
    ],
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
    ],
    exports: [
        CommonModule,
        FormsModule,
        RouterModule,
        ReactiveFormsModule,

        MatNativeDateModule,
        FormsModule,
        MatDatepickerModule,
        MatIconModule,
        MatButtonModule,
        MatCheckboxModule,
        MatToolbarModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatListModule,
        MatRadioModule,
        MatSnackBarModule,
        MatTableModule,
        MatAutocompleteModule,
        MatProgressSpinnerModule,
        MatProgressButtonsModule
    ],
    providers: [
    ]
})
export class SharedModule { }
