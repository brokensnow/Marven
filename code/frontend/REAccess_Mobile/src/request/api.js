import axios from "axios";
import qs from "qs";

// axios.defaults.baseURL = 'http://www.baidu.com/api/'  //正式
axios.defaults.baseURL = "/api/api"; //测试
//post请求头
axios.defaults.headers.post["Content-Type"] =
"application/x-www-form-urlencoded;charset=UTF-8";
//设置超时
axios.defaults.timeout = 10000;

axios.interceptors.request.use(
(config) => {
    return config;
    },
(error) => {
        return Promise.reject(error);
    }
 );
 
 axios.interceptors.response.use(
   (response) => {
     if (response.status == 200) {
       return Promise.resolve(response);
     } else {
       return Promise.reject(response);
     }
   },
   (error) => {
     console.log("请求错误！" + error);
   }
 );
 export default {
   //导出post请求
   post(url, data) {
     return new Promise((resolve, reject) => {
       axios({
         method: "post",
         url,
         data: qs.stringify(data),
       })
         .then((res) => {
           resolve(res);
         })
         .catch((err) => {
           reject(err);
         });
     });
   },
   //导出get请求
   get(url, data) {
     return new Promise((resolve, reject) => {
       axios({
         method: "get",
         url,
         params: data,
       })
         .then((res) => {
           resolve(res);
         })
         .catch((err) => {
           reject(err);
         });
     });
   },
 };