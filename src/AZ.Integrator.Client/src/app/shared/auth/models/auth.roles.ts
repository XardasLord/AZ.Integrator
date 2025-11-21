export enum AuthRoles {
  MasterAdmin = 'MASTER_ADMIN',
  Admin = 'ADMIN',
  ScannerIn = 'SCANNER_IN',
  ScannerOut = 'SCANNER_OUT',

  // Unused but required
  'offline_access' = 'offline_access',
  'default-roles-az-integrator' = 'default-roles-az-integrator',
  'uma_authorization' = 'uma_authorization',
}

export const AllRoles: AuthRoles[] = [
  AuthRoles.MasterAdmin,
  AuthRoles.Admin,
  AuthRoles.ScannerIn,
  AuthRoles.ScannerOut,
];
