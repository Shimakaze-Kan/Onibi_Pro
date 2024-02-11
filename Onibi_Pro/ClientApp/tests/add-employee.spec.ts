import { test, expect } from '@playwright/test';

test('add employee', async ({ page }) => {
  test.setTimeout(120000);
  await page.goto('https://localhost:7002/');
  await page.getByPlaceholder('Login').click();
  await page.getByPlaceholder('Login').fill('benjaminManager@mcDowell.com');
  await page.getByPlaceholder('Password').click();
  await page.getByPlaceholder('Password').fill('pass123@!');
  await page.getByRole('button', { name: 'LOGIN' }).click();
  await page.getByRole('button', { name: 'Personel Management' }).click();
  await page.waitForTimeout(18000);
  await page
    .getByRole('button', {
      name: 'Add Employee',
    })
    .click();
  await page.getByRole('textbox', { name: 'Email' }).click();
  await page.getByRole('textbox', { name: 'Email' }).fill('test@e2e.com');
  await page.getByRole('textbox', { name: 'Email' }).press('Tab');
  await page.getByRole('textbox', { name: 'First Name' }).fill('test');
  await page.getByRole('textbox', { name: 'First Name' }).press('Tab');
  await page.getByRole('textbox', { name: 'Last Name' }).fill('e2e');
  await page.getByRole('textbox', { name: 'Last Name' }).press('Tab');
  await page.getByRole('textbox', { name: 'City' }).fill('e2ecity');
  await page.locator('#mat-select-value-9').click();
  await page.getByText('Cashier', { exact: true }).click();
  await page.getByRole('option', { name: 'Cook' }).locator('span').click();
  await page.locator('.cdk-overlay-container > div:nth-child(3)').click();
  await page.getByRole('button', { name: 'I Accept' }).click();
  await page.waitForTimeout(18000);
  await page.locator('app-personel-management form').getByText('Email').click();
  await page
    .locator('app-personel-management form')
    .getByText('Email')
    .fill('test@e2e.com');
  await page.getByRole('button', { name: 'Search' }).click();
  await page.getByRole('cell', { name: 'test@e2e.com' }).click();
  await expect(page.locator('tbody')).toContainText(/test@e2e\.com/);
});
