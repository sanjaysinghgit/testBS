// Makes the first letter of the string capital.
// Sample of using: 
// var str = "mystring";
// str = str.capitalize(); // returns "Mystring"
String.prototype.capitalize = function () {
    return this.charAt(0).toUpperCase() + this.slice(1);
}