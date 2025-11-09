import { EdgeBandingTypeViewModel } from '../../../../shared/graphql/graphql-integrator.schema';

/**
 * Helper do konwersji EdgeBandingTypeViewModel na wartość liczbową dla API
 * Użyj tego helpera TYLKO jeśli backend wymaga wartości liczbowych (0, 1, 2)
 * zamiast wartości string ('NONE', 'ONE', 'TWO')
 */
export class EdgeBandingTypeHelper {
  static toNumber(type: EdgeBandingTypeViewModel): number {
    switch (type) {
      case EdgeBandingTypeViewModel.None:
        return 0;
      case EdgeBandingTypeViewModel.One:
        return 1;
      case EdgeBandingTypeViewModel.Two:
        return 2;
      default:
        return 0;
    }
  }

  static fromNumber(value: number): EdgeBandingTypeViewModel {
    switch (value) {
      case 1:
        return EdgeBandingTypeViewModel.One;
      case 2:
        return EdgeBandingTypeViewModel.Two;
      case 0:
      default:
        return EdgeBandingTypeViewModel.None;
    }
  }
}
