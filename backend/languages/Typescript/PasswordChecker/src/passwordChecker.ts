export function CheckPassword(password: string): boolean {
  if(password.length >= 6 && password.length <= 12){
      return true;
  }
  return false;
}
console.log(CheckPassword("123456"));