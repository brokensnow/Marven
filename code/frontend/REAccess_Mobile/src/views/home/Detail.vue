<template>
<div>
  <div class="detail">
     <div class="news-title">
       {{newsDetail.newsTitle}}
     </div>
      <div class="news-date">
       {{newsDetail.newsReleaseDate}}
     </div>
     <div class="news-tags">
        <span class="news-tag-title">产城智链：</span>
        <div class="news-tag-box" v-for="(itag,itagNum) in filterArray(newsDetail.newTags)" :key="itagNum">
            <div class="new-tag">{{itag}}</div>
        </div>
     </div>
      <div class="news-img-box"><img :src="newsDetail.newsImage" class="news-img"/></div>
      <div class="news-content">{{newsDetail.newsContent}}</div>
  </div>
   <div class="footer">
        <div class="footer-img">
           <!-- <a href="https://rea.deloitte.com.cn"><div class="link-web"></div></a> -->
           <!-- <img src="@/assets/查看更多资讯.svg"/> -->
           <div class="footer-img-bg">
             <div class="footer-line-box">
              <img src="@/assets/解锁.svg" class="footer-icon-size"/>
              <span class="footer-img-font">查看更多资讯</span>
             </div>
             <div class="footer-line-box1">
               <img src="@/assets/登录.svg" class="footer-icon-size"/>
               <span class="footer-img-font2">请登录PC端 rea.deloitte.com.cn</span>
             </div>
            </div>
        </div>
   </div>
</div>
</template>

<script>
import api from "@/request/api";
export default {
  name: 'Detail',
  data(){
    return{
      newsDetail:{}
    }
  },
  created(){
    const newsId = this.$route.query.id
    this.getNewDetails(newsId)
  },
  methods:{
    filterArray(array){
      return array.filter((item)=>item !=='')
    },
    getNewDetails(id){
      api
      .get(`/Rank/GetNewsById?newsId=${id}`)
      .then((res) => {
        this.newsDetail = res.data.returnObj
        this.newsDetail.newsImage = 'https://rea.deloitte.com.cn/api/'+ res.data.returnObj.newsImage
      });
    },
  }
}
</script>
<style scoped>
.detail{
  padding: 1rem 1rem 0.5rem 1rem;
  margin-bottom: 6rem;
}
.news-title{
  font-size: 1rem;
  color: #333333;
  text-align: left;
  font-weight: bold;
}
.news-card{
  background: #FFFFFF;
  box-shadow: 0 1px 20px 1px rgba(0,0,0,0.06);
  margin-bottom: 0.8rem;
  height: 5rem;
  padding: 1rem 1.5rem 1rem 1.5rem;
}
.news-img-box{
  width: 100%;
  margin-bottom: 1rem;
  margin-top: 1rem;
}
.news-img{
  width: 100%;
}
.news-content-box{
  float: left;
  width: 70%;
  text-align: left;
}
.new-title{
  font-size: .875rem;
  color: #333333;
  width: 100%;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
  padding-left: 0.5rem;
  margin-top: 0.5rem;
}
.news-content{
  font-size: .85rem;
  color: #333333;
  width: 100%;
  line-height: 1.25rem;
  text-align: left;
}
.news-tags{
  width: 100%;
}
.news-tags::after{
    content:"";
    height:0px;
    width:0px;
    clear:both;
    display:block;
    overflow: hidden;
}
.news-tag-box{
  border: .0313rem solid #4182D2;
  border-radius: 2px;
  float: left;
  padding: 0 .25rem 0 .25rem;
  margin-right: .25rem;
  margin-bottom: .2rem;
  /* overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis; */
}
.news-tag-title{
  font-size: .625rem;
  color: #333333;
  float: left;
}
.new-tag{
  font-size: .625rem;
  color: #4182D2;
}
.news-date{
  font-size: .75rem;
  color: #666666;
  text-align: left;
  line-height: 1.875rem;
}
.footer{
 text-align: center;
 width: 100%;
 position: fixed;
 bottom: 0;
 background: #f7f8fc;
 /* padding: 1rem; */
}
.footer-img >>> img{
  /* width: 94%; */
}
.footer-img{
  /* text-align: center; */
}
.link-web{
    /* background: red; */
    width: 28%;
    height: 1.5rem;
    float: left;
    margin-top: 17%;
    position: absolute;
    margin-left: 1.8rem;
    opacity: 0;
}
</style>

