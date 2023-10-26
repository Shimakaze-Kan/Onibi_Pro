// hope that i will never have to use it
import {
  Directive,
  ElementRef,
  OnDestroy,
  OnInit,
  Renderer2,
} from '@angular/core';

@Directive({
  selector: '[appTakeSpace]',
})
export class TakeSpaceDirective implements OnInit, OnDestroy {
  private _observer!: ResizeObserver;
  private _relativeElement!: HTMLElement;

  constructor(
    private readonly el: ElementRef,
    private readonly renderer: Renderer2
  ) {}

  ngOnInit(): void {
    this._observer = new ResizeObserver(() => {
      this.createRelativeElement();
    });

    this._observer.observe(this.el.nativeElement);
  }

  ngOnDestroy(): void {
    this._observer.disconnect();
  }

  createRelativeElement() {
    const parent = this.el.nativeElement.parentElement;
    const height = this.el.nativeElement.getBoundingClientRect().height;

    const existingRelativeElement = parent.querySelector('.relative-element');
    if (existingRelativeElement) {
      parent.removeChild(existingRelativeElement);
    }

    this._relativeElement = this.renderer.createElement('div');
    this.renderer.addClass(this._relativeElement, 'relative-element');
    this.renderer.setStyle(this._relativeElement, 'height', height + 'px');

    this.renderer.insertBefore(
      parent,
      this._relativeElement,
      this.el.nativeElement
    );

    const absoluteElement = this.el.nativeElement;
    const relativeTop = this._relativeElement.offsetTop;
    this.renderer.setStyle(absoluteElement, 'top', relativeTop + 'px');
  }
}
