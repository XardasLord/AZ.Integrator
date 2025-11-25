// Animacje dla komponentu integracji
import { animate, query, stagger, style, transition, trigger } from '@angular/animations';

export const integrationsAnimations = {
  // Subtelna animacja wejścia - tylko fade
  cardSlideIn: trigger('cardSlideIn', [
    transition(':enter', [style({ opacity: 0 }), animate('250ms ease-out', style({ opacity: 1 }))]),
  ]),

  // Delikatny fade dla listy
  listAnimation: trigger('listAnimation', [
    transition('* => *', [
      query(':enter', [style({ opacity: 0 }), stagger('50ms', [animate('200ms ease-out', style({ opacity: 1 }))])], {
        optional: true,
      }),
    ]),
  ]),

  // Fade in/out
  fadeInOut: trigger('fadeInOut', [
    transition(':enter', [style({ opacity: 0 }), animate('200ms', style({ opacity: 1 }))]),
    transition(':leave', [animate('150ms', style({ opacity: 0 }))]),
  ]),
};
