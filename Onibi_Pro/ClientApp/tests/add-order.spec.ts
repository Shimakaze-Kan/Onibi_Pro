import { test, expect } from '@playwright/test';

test('add cheeseburger deluxe order', async ({ page }) => {
  test.setTimeout(120000);
  await page.goto('https://localhost:7002/');
  await page.getByPlaceholder('Login').click();
  await page.getByPlaceholder('Login').fill('emilyManager@mcDowell.com');
  await page.getByPlaceholder('Password').click();
  await page.getByPlaceholder('Password').fill('pass123@!');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.goto('https://localhost:7002/welcome');
  await page.getByRole('button', { name: 'Order' }).click();
  await page.getByRole('button', { name: 'Place Order' }).click();
  await page.getByRole('button', { name: 'Add Product' }).click();
  await page.locator('#mat-select-value-3').click();
  await page
    .getByLabel('Fast Food Fiesta')
    .getByText('Cheeseburger Deluxe')
    .click();
  await page.getByRole('button', { name: 'I Accept' }).click();
  await page.waitForTimeout(10000);
  await page.locator('td').filter({ hasText: 'expand_more' }).first().click();
  await expect(page.locator('tbody')).toContainText('Cheeseburger Deluxe');
});
