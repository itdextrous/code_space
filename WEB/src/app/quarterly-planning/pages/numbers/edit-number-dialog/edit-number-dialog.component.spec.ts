import { ComponentFixture, TestBed } from "@angular/core/testing";

import { EditNumberDialogComponent as EditNumberDialogComponent } from "./edit-number-dialog.component";

describe("EditNumberDialogComponent", () => {
    let component: EditNumberDialogComponent;
    let fixture: ComponentFixture<EditNumberDialogComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [ EditNumberDialogComponent ]
        })
            .compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(EditNumberDialogComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it("should create", () => {
        expect(component).toBeTruthy();
    });
});
