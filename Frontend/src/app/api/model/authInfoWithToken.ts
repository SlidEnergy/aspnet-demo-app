/**
 * SharpDevBackend
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */


export interface AuthInfoWithToken {
    /**
     * Токен для доступа к Api
     */
    accessToken: string;
    /**
     * Время, через которое истекет время действия токена
     */
    expiresIn: number;
    /**
     * Отображаемое имя пользователя
     */
    userName: string;
    /**
     * Json строка. Флаг, указывающий, что пользователь является администратором
     */
    isAdmin: string;
    /**
     * Json строка. Массив доступных разрешений пользователю
     */
    permissions: string;
    /**
     * Дата выпуска токена
     */
    issued: string;
    /**
     * Дата истечения действия токена
     */
    expires: string;
}
