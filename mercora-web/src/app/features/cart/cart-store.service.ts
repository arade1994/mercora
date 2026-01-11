import { computed, Injectable, signal } from '@angular/core';
import { CartItem } from './cart.models';

@Injectable({
  providedIn: 'root',
})
export class CartStore {
  private readonly _items = signal<CartItem[]>([]);

  readonly items = computed(() => this._items());
  readonly itemCount = computed(() => this._items().reduce((sum, i) => sum + i.quantity, 0));
  readonly currencyCode = computed(() => this._items()[0]?.currencyCode ?? 'EUR');
  readonly subtotal = computed(() => this._items().reduce((sum, i) => sum + i.unitPrice * i.quantity, 0));

  clear() {
    this._items.set([]);
  }

  remove(variantId: number) {
    this._items.update(items => items.filter(i => i.variantId !== variantId));
  }

  setQuantity(variantId: number, quantity: number) {
    const q = Math.max(1, Math.floor(quantity || 1));

    this._items.update(items => 
      items.map(i => {
        if (i.variantId !== variantId) return i;

        const max = i.maxQuantity;
        const finalQty = max !== null ? Math.min(q, max!) : q;

        return {...i, quantity: finalQty};
      }) 
    )
  }

  add(item: CartItem) {
    this._items.update(items => {
      const existing = items.find(i => i.variantId === item.variantId);

      if (!existing) {
        const max = item.maxQuantity;
        const qty = max != null ? Math.min(item.quantity, max) : item.quantity;
        return [...items, { ...item, quantity: Math.max(1, qty) }];
      }

      const max = existing.maxQuantity;
      const nextQty = existing.quantity + item.quantity;
      const finalQty = max != null ? Math.min(nextQty, max) : nextQty;

      return items.map(i =>
        i.variantId === item.variantId ? { ...i, quantity: finalQty } : i
      );
    });
  }
}
