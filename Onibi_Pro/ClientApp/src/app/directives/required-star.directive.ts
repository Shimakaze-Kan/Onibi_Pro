import { Directive, ElementRef, Renderer2, AfterViewInit } from '@angular/core';

@Directive({
  selector: 'label[appRequiredStar]',
})
export class RequiredStarDirective implements AfterViewInit {
  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit() {
    this.checkAncestorsForRequired(this.el.nativeElement.parentElement);
  }

  private checkAncestorsForRequired(element: HTMLElement) {
    const hasRequiredChild = Array.from(element.children).some((child) => {
      return (
        child.getAttribute('aria-required') === 'true' ||
        this.checkAncestorsForRequired(child as HTMLElement)
      );
    });

    if (hasRequiredChild) {
      this.addAsterisk();
    }
  }

  private addAsterisk() {
    const textContent = this.el.nativeElement.textContent;
    this.renderer.setProperty(
      this.el.nativeElement,
      'textContent',
      textContent + '*'
    );
  }
}
