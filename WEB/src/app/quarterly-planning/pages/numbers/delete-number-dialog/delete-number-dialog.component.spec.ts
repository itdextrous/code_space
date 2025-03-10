import { ComponentFixture, TestBed } from "@angular/core/testing";

import { DeleteNumberDialogComponent } from "./delete-number-dialog.component";

describe("DeleteNumberDialogComponent", () => {
    let component: DeleteNumberDialogComponent;
    let fixture: ComponentFixture<DeleteNumberDialogComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [DeleteNumberDialogComponent]
        })
            .compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(DeleteNumberDialogComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it("should create", () => {
        expect(component).toBeTruthy();
    });
});
