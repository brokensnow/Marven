<template>
  <div class="home" id="content">
     <div class="news-title">
       资讯
     </div>
      <div class="news-card" v-for="item in newsList" :key="item.id" @click="toNewDetail(item)">
        <div class="news-img-box"><img :src="item.newsImage" class="news-img"/></div>
        <div class="news-content-box">
            <div class="new-title" v-html="htmlDecode(item.newsTitle)">{{htmlDecode(item.newsTitle)}}
              <div class="news-date" >{{item.newsReleaseDate}}</div>
            </div>
            <!-- <div class="news-date" >{{item.newsReleaseDate}}</div> -->
            <!-- <div class="news-content">{{item.newsContent}}</div> -->
            <div class="news-buttom-box">
              <div class="news-tag-title">产城智链：</div>
              <!-- <div class="new-tag-box"> -->
                <div class="news-tag-box" v-for="(itag,itagNum) in filterArray(item.newTags)" :key="itagNum">
                    <div class="new-tag">{{itag}}</div>
                </div>
              <!-- </div> -->
            </div>
        </div>
      </div>
      <div v-loading="loading" class="loading-box">
      </div>
      <div class="footer">
            <div class="footer-img">
           <!-- <a href="https://rea.deloitte.com.cn"><div class="link-web"></div></a> -->
           <!-- <img src="@/assets/解锁城市.svg"/> -->
            <div class="footer-img-bg">
             <div class="footer-line-box">
              <img src="@/assets/解锁.svg" class="footer-icon-size"/>
              <span class="footer-img-font">解锁更多资讯</span>
             </div>
             <div class="footer-line-box1">
               <img src="@/assets/登录.svg" class="footer-icon-size"/>
               <span class="footer-img-font2">请登录PC端 rea.deloitte.com.cn</span>
             </div>
            </div>
        </div>
        <span style="font-size: .625rem;color: #2C2C2C;">沪ICP备14034737号-3</span>
        </div>
  </div>
</template>

<script>
import api from "@/request/api";
export default {
  name: 'Home',
  data(){
    return{
      newsList:[],
      currentPage: 1,
      pageSize: 5,
      loading:false,
      loadingContent:null,
    }
  },
  created(){
    this.getNewList()
  },
  mounted() {
    window.addEventListener("scroll", this.scrollBottom, true);
  },
  methods:{
    filterArray(array){
      return array.filter((item)=>item !=='')
    },
    scrollBottom() {
    // 滚动到页面底部时
        const el = document.getElementById("content");
        const windowHeight = window.screen.height;
        const scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        const contentHeight = el.clientHeight;
        const toBottom = contentHeight - windowHeight - scrollTop;
        if (toBottom < -360 && !this.loadingContent) {
        // console.log(toBottom)
          this.loading = true;
          // 请求的数据未加载完成时
          this.currentPage = this.currentPage+1
          this.getNewList();
        }else{
           this.loading = false;
        }
    },
    getNewList(){
      api
      .get(`/Rank/GetNewsList?currentPage=${this.currentPage}&pageSize=${this.pageSize}`)
      .then((res) => {
        // console.log('-----',res.data);
        const data = res.data.returnObj
        if(data.length !== 0){
          data.map((item)=>{
              this.newsList.push(item)
          })
          this.newsList.map((item)=>{
            if(item.newsImage.indexOf("http://reaapi.ftechsoftware.com/") === -1){
                item.newsImage = 'http://reaapi.ftechsoftware.com/'+item.newsImage
            }
          })
        }else{
           this.loadingContent ="没有更多资讯"
        }
      });
    },
    toNewDetail(obj){
      this.$router.push({path: `/Detail?id=${obj.id}`})
    },
    getMargin(index){
      this.$nextTick(() => {
        const titlelist = document.getElementsByClassName('new-title')
        const height = titlelist[index].offsetHeight / 16
        return 'margin-left:' + height + 'rem'   
      })
    },
    htmlDecode(str){
      str = str.replaceAll("div","span")
      const div = document.createElement('div');
      div.innerHTML = str;
      return div.innerText || div.innerHTML;
    }
  }
}
</script>
<style scoped>
.news-title{
  font-size: 1.125rem;
  color: #000000;
  text-align: left;
  padding: 1rem 1rem 0.5rem 1rem;
  font-weight: bold;
}
.news-card{
  background: #FFFFFF;
  box-shadow: 0 1px 20px 1px rgba(0,0,0,0.06);
  margin-bottom: 0.8rem;
  height: 5.2rem;
  /* width: 100%; */
  padding: 0.8rem;
}
.news-card::after{
    content:"";
    height:0px;
    width:0px;
    clear:both;
    display:block;
    overflow: hidden;
}
.news-img-box{
  /* width: 33%; */
  vertical-align: middle;
  height: 100%;
  float: left;
}
.news-img{
  width: 7.5rem;
  height: 5.2rem;
  line-height: 6.25rem;
  border-radius: .125rem;
}
.news-content-box{
  float: left;
  width: 62%;
  text-align: left;
  height: 5.2rem;
  position: absolute;
  margin-left: 7.5rem;
  padding-right: 0.5rem;
}
.new-title{
  font-size: .875rem;
  color: #333333;
  width: 100%;
  padding-left: 0.5rem;
  margin-top: -0.1rem;
  font-weight: bold;
  line-height: 1.4rem;
}
.new-title >>> span{
    display: inherit;
}
.new-title::after{
    content:"";
    height:0px;
    width:0px;
    clear:both;
    display:block;
    overflow: hidden;
}
.news-content{
  font-size: .625rem;
  color: #999999;
  width: 100%;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
  padding-left: 0.5rem;
  margin-bottom: 1rem;
  margin-top: .5rem;
}
.news-buttom-box{
  padding-left: 0.5rem;
  position: absolute;
  /* margin-top: 0.1rem; */
  bottom: 0;
  /* margin-top: 0.4rem; */
  /* position: absolute; */
  /* margin-bottom: 2rem; */
  /* width: 80%; */
  /* height: 1.25rem; */
  /* overflow: hidden; */
  /* overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis; */
}
.new-tag-box{
  /* width: 100%; */
  /* overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis; */
  float: left;
}
.news-buttom-box::after{
    content:"";
    height:0px;
    width:0px;
    clear:both;
    display:block;
    overflow: hidden;
}
.news-tag-box{
  border: .0313rem solid #999999;
  border-radius: 2px;
  float: left;
  padding: 0 .25rem 0 .25rem;
  margin-right: .25rem;
  /* margin-bottom: 0.1rem; */
  /* overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis; */
}
.news-tag-title{
  font-size: .7rem;
  color: #333333;
  float: left;
  font-weight: bold;
  height: 1.18rem;
}
.new-tag{
  font-size: .65rem;
  color: #999999;
}
.news-date{
  font-size: .625rem;
  color: #B0B0B0;
  text-align: right;
  position: relative;
  float: right;
  margin-right: 0.2rem;
  /* overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
  margin-top: -1rem; */
}
.footer{
  font-size: .625rem;
  color: #2C2C2C;
  margin-top: 3rem;
  margin-bottom: 2rem;
}
.loading-box {
    font-size: .75rem;
    color: #999999;
    /* margin-top: 29px; */
}
.footer-icon-size{
  margin-top: 0.1rem;
}
.footer-line-box {
    padding-top: 2.5rem;
    margin-bottom: 0.2rem;
}
</style>
